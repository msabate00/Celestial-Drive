using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats
{
    public event EventHandler OnStatsChanged;

    public static int STAT_MIN = 0;
    public static int STAT_MAX = 255;

    public enum Type { 
        Health, 
        Damage,
        Power,
        Mobility,
        Speed
    }

    private SingleStat healthStat;
    private SingleStat damageStat;
    private SingleStat powerStat;
    private SingleStat mobilityStat;
    private SingleStat speedStat;

    public Stats(int healthStatAmount, int damageStatAmount, int powerStatAmount, int mobilityStatAmount, int speedStatAmount) {
        healthStat = new SingleStat(healthStatAmount);
        damageStat = new SingleStat(damageStatAmount);
        powerStat = new SingleStat(powerStatAmount);
        mobilityStat = new SingleStat(mobilityStatAmount);
        speedStat = new SingleStat(speedStatAmount);
    }

    private SingleStat GetSingleStat(Type statType) {
        switch (statType) {
            default:
            case Type.Health:       return healthStat;
            case Type.Damage:       return damageStat;
            case Type.Power:        return powerStat;
            case Type.Mobility:     return mobilityStat;
            case Type.Speed:        return speedStat;
        }
    }

    public void SetStatAmount(Type statType,int statAmount)
    {
        GetSingleStat(statType).SetStatAmount(statAmount);
        if (OnStatsChanged != null) OnStatsChanged(this, EventArgs.Empty);
    }

    public void AddStatAmount(Type statType, int amount)
    {
        SetStatAmount(statType, GetStatAmount(statType) + amount);
    }

    public int GetStatAmount(Type statType)
    {
        return GetSingleStat(statType).GetStatAmount();
    }

    public float GetStatAmountNormalized(Type statType)
    {
        return GetSingleStat(statType).GetStatAmountNormalized();
    }

    


    private class SingleStat {


        private int stat;

        public SingleStat(int statAmount) {
            SetStatAmount(statAmount);
        }


        public void SetStatAmount(int statAmount)
        {
            stat = Mathf.Clamp(statAmount, STAT_MIN, STAT_MAX);
        }

        public int GetStatAmount()
        {
            return stat;
        }

        public float GetStatAmountNormalized()
        {
            return (float)stat / STAT_MAX;
        }


    }


  
}
