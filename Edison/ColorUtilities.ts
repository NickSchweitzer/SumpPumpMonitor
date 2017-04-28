export class RGB {
    constructor(public R: number, public G: number, public B: number) { }

    static ColorNameToRGB(color: string): RGB {
        switch (color) {
            case 'Red':
                return new RGB(255, 0, 0);
            case 'Green':
                return new RGB(0, 255, 0);
            default:
                return new RGB(255, 255, 255);
        }
    }
}