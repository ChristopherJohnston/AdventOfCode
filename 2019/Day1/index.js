data = require('./data');

function calc_total_fuel(masses) {
    var total_fuel = 0;

    for (let i=0;i<masses.length;i++) {
        let mass = masses[i];

        do {
            mass = Math.max(Math.floor(mass/3.0) - 2, 0);
            total_fuel += mass;
        } while (mass > 0);
    }
    return total_fuel;
}

console.log(calc_total_fuel(data.input));