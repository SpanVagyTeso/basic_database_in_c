import json
import sys

class EndPoint():
    
    def __init__(self,name):
        self.name = name
        self.connections = dict()
    
    def add_connection(self,connectionPoint,capacity):
        self.connections[connectionPoint]=[capacity,0]
    
    def remove_demand(self,CP,demand):
        self.connections[CP][1]-=demand

    def add_demand(self,CP,demand):
        self.connections[CP][1]+=demand

class SwitchPoint():
    def __init__(self,name):
        self.name = name
        self.connections = dict()
    
    def add_connection(self,connectionPoint,capacity):
        self.connections[connectionPoint]=[capacity,0]

    def add_demand(self,CP,demand):
        self.connections[CP][1]+=demand

    def remove_demand(self,CP,demand):
        self.connections[CP][1]-=demand

switches = dict()
configs = None

def book(end_points,demand):
    route= None
    for poss in configs['possible-circuits']:
        temp_ep=[poss[0], poss[-1]]
        if temp_ep == end_points :
            i = 0
            is_free = True
            while i < len(poss)-1 and is_free:
                current = switches[poss[i]].connections[poss[i+1]]
                if current[0]< current[1]+demand:
                    is_free = False
                i+=1
            if is_free:
                i = 0
                while i < len(poss)-1:
                    current = switches[poss[i]].connections[poss[i+1]]
                    switches[poss[i]].add_demand(poss[i+1],demand)
                    switches[poss[i+1]].add_demand(poss[i],demand)
                    i+=1
                return poss

    
    return route

def free(demand):
    if demand['route'] == None :
        return False
    i = 0
    while i < len(demand['route'])-1:
        switches[demand['route'][i]].remove_demand(demand['route'][i+1],demand['demand'])
        switches[demand['route'][i+1]].remove_demand(demand['route'][i],demand['demand'])
        i+=1
    return True


if __name__  == "__main__":
    
    with open(sys.argv[1]) as json_file:
        configs=json.load(json_file)
        for EP in configs['end-points']:
            switches[EP] = EndPoint(EP)
        for SP in configs['switches']:
            switches[SP] = SwitchPoint(SP)
        #print(switches.keys())
        
        for link in configs['links']:
            switches[link['points'][0]].add_connection(link['points'][1],link['capacity'])
            switches[link['points'][1]].add_connection(link['points'][0],link['capacity'])

        duration = configs['simulation']['duration']
        demands = configs['simulation']['demands']
        for i in range (0,duration+1):
            for demand in demands:
                if demand['end-time'] == i:
                    if free(demand):
                        print(f"igény felszabadítás: {demand['end-points'][0]}<->{demand['end-points'][1]} st:{i} - sikeres")
                    pass
                
                if demand['start-time'] == i:
                    route = book(demand['end-points'],demand['demand']) 
                    demand['route']=route
                    if  route != None:
                        print(f"igény bookás: {demand['end-points'][0]}<->{demand['end-points'][1]} st:{i} - sikeres")
                    else:
                        print(f"igény bookás: {demand['end-points'][0]}<->{demand['end-points'][1]} st:{i} - sikertelen")
                    
