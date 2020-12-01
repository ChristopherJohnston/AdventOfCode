data = require('./data');

function program_alarm(input, noun, verb) {
    pos = 0;
    input[1] = noun;
    input[2] = verb;

    while(true) {
        code = input[pos];
        location1 = input[pos+1];
        value1 = input[location1];
        location2 = input[pos+2];
        value2 = input[location2];
        location_output = input[pos+3];

        if (code == 99) {
            console.log('99');
            break;
        }
        else if (code == 1) {
            input[location_output] = input[location1] + input[location2];
        }
        else if (code == 2) {
            input[location_output] = input[location1] * input[location2];
        }
        else {
            return 'error';
        }
        console.log(`${code}, ${value1}, ${value2}, ${location_output} -> ${input[location_output]}`);
        pos +=4;
    }
    return input[0];
}

function gravity_calc() {
    const target = 19690720;

    for (var noun=0; noun<100; noun++) {
        for (var verb=0; verb<100; verb++) {
            if (program_alarm([...data.input], noun, verb) == target) {
                console.log(`Found target. Noun: ${noun}, Verb: ${verb}`);
                return 100*noun+verb;
            }
        }
    }

}

// console.log(program_alarm([...data.input], 12, 2));
console.log(gravity_calc());