data = require('./data');

function calc_manhattan_distance(x0, y0, x1, y1) {
    return Math.abs(x0-x1) + Math.abs(y0-y1);
}

function get_steps(wire) {
    let steps = {};

    let x=0;
    let y=0;
    let dirX = 0;
    let dirY = 0;
    total_distance = 0;

    for (let i=0; i<wire.length; i++) {
        let direction = wire[i][0];
        let distance = parseInt(wire[i].slice(1, wire[i].length));

        if (direction == 'R') {
            dirX = 1;
            dirY = 0;
        }
        else if (direction == 'L') {
            dirX = -1;
            dirY = 0;
        }
        else if (direction == 'U') {
            dirX = 0;
            dirY = 1;
        }
        else if (direction == 'D') {
            dirX = 0;
            dirY = -1;
        }

        for (let d=0; d<distance; d++) {
            x += dirX;
            y += dirY;
            total_distance += 1;

            if (steps[[x,y]] == undefined) {
                steps[[x, y]] = total_distance;
            }
        }
    }
    return steps;
}

// function get_intersections(wires) {
//     let paths = [];
//     let intersections = {};

//     for (var line=0; line<wires.length; line++) {
//         let x = 0;
//         let y = 0;
//         let dirX = 0;
//         let dirY = 0;
//         paths.push([]);

//         for (let i = 0; i < wires[line].length; i++) {
//             let direction = wires[line][i][0];
//             let distance = parseInt(wires[line][i].slice(1, wires[line][i].length));

//             if (direction == 'R') {
//                 dirX = 1;
//                 dirY = 0;
//             }
//             else if (direction == 'L') {
//                 dirX = -1;
//                 dirY = 0;
//             }
//             else if (direction == 'U') {
//                 dirX = 0;
//                 dirY = 1;
//             }
//             else if (direction == 'D') {
//                 dirX = 0;
//                 dirY = -1;
//             }

//             for (let d=0; d<distance; d++) {
//                 x += dirX;
//                 y += dirY;

//                 if (line == 0) {
//                     if (paths[line][[x,y]]) {
//                         paths[line][ [x, y] ] += 1;
//                     }
//                     else
//                     {
//                         paths[line][ [x,y] ] = 1;
//                     }
//                 }
//                 else {
//                     if (paths[0][[x,y]] !== undefined) {
//                         let manhattan_dist = calc_manhattan_distance(0, 0, x, y);
//                         if (manhattan_dist != 0) {
//                             intersections[[x,y]] = manhattan_dist;
//                         }
//                     }
//                 }
//             }
//         }
//     }
//     return intersections;
// }

function get_intersections(wires) {
    let intersections = {};

    let paths1 = get_steps(wires[0]);
    let paths2 = get_steps(wires[1]);

    for (let key of Object.keys(paths2)) {
        if (paths1[key] !== undefined) {
            k = key.split(',');
            let manhattan_dist = calc_manhattan_distance(0, 0, parseInt(k[0]), parseInt(k[1]));
            if (manhattan_dist != 0) {
                intersections[key] = manhattan_dist;
            }
        }
    }
    return intersections;
}

function get_min_distance(wires) {
    let intersections = get_intersections(wires);
    let min_dist = Number.MAX_SAFE_INTEGER;
    for (let key of Object.keys(intersections)) {
        min_dist = Math.min(min_dist, intersections[key]);
    }
    return min_dist;
}

function get_min_path(wires) {
    let intersections = get_intersections(wires);
    let paths1 = get_steps(wires[0]);
    let paths2 = get_steps(wires[1]);

    // console.log(intersections);
    // console.log(paths1);
    // console.log(paths2);

    let min_steps = Number.MAX_SAFE_INTEGER;

    for (let key of Object.keys(intersections)) {
        min_steps = Math.min(min_steps, paths1[key] + paths2[key]);
    }
    return min_steps;
}

test_data = [
    ['R75','D30','R83','U83','L12','D49','R71','U7','L72'],
    ['U62','R66','U55','R34','D71','R55','D58','R83']
];

console.log(get_min_distance(data.input));
console.log(get_min_distance(test_data));

console.log(get_min_path(test_data));
console.log(get_min_path(data.input));