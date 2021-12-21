import sys
import socket
import random
import struct
import time

server_addr = sys.argv[1]
server_port = int(sys.argv[2])

sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

sock.connect( (server_addr, server_port) )


packer = struct.Struct('c i')


guessed_number = 100
guessed_operator = b'<'

parsed_result= ""
min_numb=0
max_numb=guessed_number
prev_guessed_number=0

while parsed_result !='Y' and parsed_result !='K' and parsed_result != 'V':
	sleep_time = random.randint(1,5)
	if prev_guessed_number == guessed_number:
		#print("here")
		guessed_operator=b'='
	msg=packer.pack(guessed_operator,guessed_number)
	sock.sendall(msg)
	try:
		msg = sock.recv(packer.size)
		parsed_msg = packer.unpack( msg )
		parsed_result=parsed_msg[0].decode()
	except:
		pass

	if parsed_result == 'I':
		if guessed_operator.decode() == '<':
			max_numb=guessed_number
			prev_guessed_number=guessed_number
			guessed_number=int((min_numb+max_numb)/2)
		elif guessed_operator.decode() == '>':
			min_numb=guessed_number
			prev_guessed_number=guessed_number
			guessed_number=int((min_numb+max_numb)/2)
	elif parsed_result == 'N':
		if guessed_operator.decode() == '<':
			min_numb=guessed_number
			prev_guessed_number=guessed_number
			guessed_number=int((min_numb+max_numb)/2)
		elif guessed_operator.decode() == '>':
			max_numb=guessed_number
			prev_guessed_number=guessed_number
			guessed_number=int((min_numb+max_numb)/2)
	elif parsed_result == 'Y':
		print("Nyertem")
	elif parsed_result == 'V':
		print("Vesztettem")
		sock.close()
	#print(str(guessed_number) +" "+str(min_numb)+" "+str(max_numb))
	time.sleep(sleep_time)
			

sock.close()


