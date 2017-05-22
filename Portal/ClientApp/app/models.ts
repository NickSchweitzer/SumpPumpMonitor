export interface SumpPump {
    pumpId: string;
    desired: PumpConfiguration;
    reported: PumpConfiguration;
    lastPoint: DataPoint;
    data: string;
    alerts: string;
}

export interface PumpConfiguration {
    name: string; 
    maxWaterLevel: number;
    maxRunTimeNoChange: number;
}

export interface DataPoint {
    pumpId: string;
    timeStamp: string;
    waterLevel: number;
    pumpRunning: boolean;
}

export interface Alert {
    pumpId: string;
    timeStamp: string;
    waterLevel: number;
    pumpRunning: boolean;
    type: string;
}

type Durations = 'seconds' |
    'minutes' |
    'hours' |
    'days' |
    'weeks' |
    'months' |
    'years';