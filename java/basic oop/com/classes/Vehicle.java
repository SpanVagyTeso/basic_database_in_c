package com.classes;
import com.abstracts.Moveable;
/**
@author RFXQHM
*/
public class Vehicle extends Moveable{
    protected String name;
    protected int passangers;
    protected int tires;
    /**
    @param String név és int kerekek számát várja
    */
    public Vehicle(String name,int tires){
        this.tires=tires;
        this.name=name;
    }
    public String getVehicle(){
        return name;
    }
    public int getPassangers(){
        return passangers;
    }
    public int getTires(){
        return tires;
    }
    public void setPassangers(int p){
        passangers=p;
    }
    public void setTires(int m){
        tires=m;
    }
    public void setName(String n){
        name=n;
    }

}
