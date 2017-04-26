import {SumpPump} from "./models"

export class PumpUpdated {
    public pump: SumpPump;

    constructor(pump: SumpPump) {
        this.pump = pump;
    }
}

export class PumpSelected {
    public pump: SumpPump;

    constructor(pump: SumpPump) {
        this.pump = pump;
    }
}