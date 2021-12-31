package com.abstracts;
/**
@author RFXQHM
Ez egy absztrakt osztály
*/
public abstract class Moveable{
    protected int maxSpeed;

    public int getMaxSpeed(){
        return maxSpeed;
    }
    /**
    Ez miatt
    */
    public abstract String getVehicle();
}
