import select
import socket
import sys
import struct
import random




def fibonacci(index,stored):
    if index == 0 or index == 1:
        return 1
    elif index in stored:
        return stored[index]
    stored[index] = fibonacci(index-1,stored)+fibonacci(index-2,stored)
    return stored[index]

class SimpleTCPSelectServer:
    def __init__(self, addr='localhost', port=10001, timeout=1):
        self.server = self.setupServer(addr, port)
        # Sockets from which we expect to read
        self.inputs = [self.server]
        self.numbers = dict()
        # Wait for at least one of the sockets to be ready for processing
        self.timeout = timeout
        self.fibo = {}
        self.requests = {}
        self.packer = struct.Struct('c i')

    def setupServer(self, addr, port):
        # Create a TCP/IP socket
        server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        server.setblocking(0)
        server.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)

        # Bind the socket to the port
        server_address = (addr, port)
        server.bind(server_address)

        # Listen for incoming connections
        server.listen(5)
        return server

    def handleNewConnection(self, sock):
        print("New Input!")
        # A "readable" server socket is ready to accept a connection
        connection, client_address = sock.accept()
        connection.setblocking(0)  # or connection.settimeout(1.0)
        self.inputs.append(connection)
        self.numbers[connection]=random.randint(1,101)

    def handleDataFromClient(self, sock):
        print("hey")
        data = sock.recv(self.packer.size)
        #print(sock)
        
        
        if data:
            parsed_msg = self.packer.unpack(data)
            #print(parsed_msg)
            op = parsed_msg[0].decode()
            print(op)
            numb = parsed_msg[1]
            secret_number=self.numbers[sock]
            if op == '<':
                if secret_number < numb:
                    msg = self.packer.pack(b'I',0)
                    sock.sendall(msg)
                else:
                    msg = self.packer.pack(b'N',0)
                    sock.sendall(msg)
            elif op=='>':
                if secret_number > numb:
                    msg = self.packer.pack(b'I',0)
                    sock.sendall(msg)
                else:
                    msg = self.packer.pack(b'N',0)
                    sock.sendall(msg)
            elif op=='=':
                if secret_number == numb:
                    msg = self.packer.pack(b'Y',0)
                    sock.sendall(msg)
                    self.inputs.remove(sock)
                    self.close_all_connections()
                    sock.close()
                else:
                    msg = self.packer.pack(b'K',0)
                    sock.sendall(msg)
            
        else:
            # Interpret empty result as closed connection
            # Stop listening for input on the connection
            self.inputs.remove(sock)
            sock.close()

    def handleInputs(self, readable):
        for sock in readable:
            if sock is self.server:
                self.handleNewConnection(sock)
            else:
                self.handleDataFromClient(sock)

    def close_all_connections(self):
        for sock in self.inputs:
            # Stop listening for input on the connection
            self.inputs.remove(sock)
            msg = self.packer.pack(b'V',0)
            sock.sendall(msg)

    def handleExceptionalCondition(self, exceptional):
        for sock in exceptional:
            # Stop listening for input on the connection
            self.inputs.remove(sock)
            sock.close()

    def handleConnections(self):
        while self.inputs:
            try:
                readable, writable, exceptional = select.select(
                    self.inputs, [], self.inputs, self.timeout)

                if not (readable or writable or exceptional):
                    continue

                self.handleInputs(readable)
                self.handleExceptionalCondition(exceptional)
            except KeyboardInterrupt:
                print("A szerver leáll")
                for c in self.inputs:
                    c.close()
                self.inputs = []

if len(sys.argv) == 3:
	simpleTCPSelectServer = SimpleTCPSelectServer(sys.argv[1],int(sys.argv[2]))
else:
	simpleTCPSelectServer = SimpleTCPSelectServer()
simpleTCPSelectServer.handleConnections()