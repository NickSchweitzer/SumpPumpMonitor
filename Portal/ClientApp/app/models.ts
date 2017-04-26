export interface SumpPump {
    pumpId: string;
    name: string;
    maxWaterLevel: number;
    maxRunTimeNoChange: number;
    currentWaterLevel: number;
    pumpRunning: boolean;
    lastDataRecorded: string;   // Timestamp
    inError: boolean;
    data: string;
}

export interface DataPoint {
    pumpId: string;
    timeStamp: string;
    waterLevel: number;
    pumpRunning: boolean;
}

type Durations = 'seconds' |
    'minutes' |
    'hours' |
    'days' |
    'weeks' |
    'months' |
    'years';