export function padString(str : string, pad : string, left : boolean) : string {
    var padStr = str.toString();
    while (padStr.length < 16) {
        if (left === true) {
            padStr = pad + padStr;
        } else {
            padStr = padStr + pad;
        }
    }
    return padStr;
}