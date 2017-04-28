export class SumpPump {
    public waterLevel: number;
    public pumpOn: boolean;
    public state: PumpState;
}

export enum PumpState {
    Off,
    Running,
    StuckFull,
    StuckEmpty
}