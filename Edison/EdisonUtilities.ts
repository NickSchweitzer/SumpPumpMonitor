import { padString } from './StringUtilities';
import { RGB } from './ColorUtilities';

export function updateScreen(my, line1: string, line2: string, color: string) {
    var rgb = RGB.ColorNameToRGB(color);

    my.screen.setColor(rgb.R, rgb.G, rgb.B);
    my.screen.setCursor(0, 0);
    my.screen.write(padString(line1, ' ', false));
    my.screen.setCursor(1, 0);
    my.screen.write(padString(line2, ' ', false));
}

export class AtoDConverter {

    constructor(public resolution: number, public systemVoltage: number) { }

    digitalToVoltage(analogReading : number) : number {
        return analogReading * this.systemVoltage / this.resolution;
    }

    voltageToResistance(vOut : number, knownR : number) : number {
        return knownR / (this.systemVoltage / vOut - 1.0);
    }
}