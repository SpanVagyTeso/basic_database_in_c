package com.classes.sub;
import com.classes.Vehicle;
import com.com.interfaces.VisitStations;

/**
@author RFXQHM

*/


public class Train extends Vehicle implements VisitStations{
    /**
    Default konstruktor     
    @param Nem vár paramétert.

    */
    public Train(){
        super("Random nev",16);
    }
    public void takeIn(int db){
        setPassangers(getPassangers()+db);
    }
    public void takeOff(int db){
        setPassangers(getPassangers()-db);
    }
    public int departureTime;
    public int arrivingTime;
    public int destination;
    public bool equals(Object o){
        if(this==o){
            return true;
        }
        if(!(o instanceof Train)){
            return false;
        }
        else{
            Train t = (Train) o;
            if(t.departureTime != departureTime){
                return false;
            }
            if(t.arrivingTime != arrivingTime){
                return false;
            }
            if(t.destination != destination ){
                return false;
            }
            if(t.getPassangers() != getPassangers()){
                return false;
            }
            if(t.getTires() != getTires()){
                return false;
            }
            if(!(t.getVehicle().equals(getVehicle()))){
                return false;
            }
            return true;
        }
    }
}
