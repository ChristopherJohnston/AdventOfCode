import data
import itertools

class Operation:
    code = None
    num_args = 0
    inputs = None
    relative_base = None

    def __init__(self, memory, address, modes, inputs, relative_base):
        self.memory = memory
        self.address = address
        self.modes = modes
        self.args = []
        self.inputs = inputs
        self.relative_base = relative_base

        if self.num_args > 0:
            for i in range(0, self.num_args):
                self.args.append(self.memory[self.address+i+1])
                if len(self.modes) < i+1:
                    self.modes.append('0')
        # print(self.memory)
        # print(self.code)
        # print(self.args)
        # print(self.modes)

    def next_address(self):
        return self.address + self.num_args + 1
    
    def execute(self):
        raise NotImplementedError('execute is not implemented')

    def read_arg(self, i):
        if self.modes[i] == '0':
            return self.memory[self.args[i]]
        elif self.modes[i] == '2':
            return self.memory[self.relative_base + self.args[i]]
        else:
            return self.args[i]
    
    def write_arg(self, i, v):
        if self.modes[i] == '0':
            # print('mode 0 write')
            self.memory[self.args[i]] = v
        elif self.modes[i] == '2':
            # print('mode 2 write')
            self.memory[self.relative_base + self.args[i]] = v
        else:
            print('invalid write')

class Add(Operation):
    code = 1
    num_args = 3

    def __init__(self, memory, address, modes, inputs, relative_base):
        super(Add, self).__init__(memory, address, modes, inputs, relative_base)

    def execute(self):
        value1 = self.read_arg(0)
        value2 = self.read_arg(1)
        res = value1 + value2
        # self.memory[self.args[2]] = res
        self.write_arg(2, res)
        # print('{0}, {1}, {2}, {3} -> {4}'.format(self.code, value1, value2, self.args[2], self.memory[self.args[2]]))
        return res

class Multiply(Operation):
    code = 2
    num_args = 3

    def __init__(self, memory, address, modes, inputs, relative_base):
        super(Multiply, self).__init__(memory, address, modes, inputs, relative_base)
    
    def execute(self):
        value1 = self.read_arg(0)
        value2 = self.read_arg(1)
        res = value1 * value2
        # self.memory[self.args[2]] = res
        self.write_arg(2, res)
        # print('{0}, {1}, {2}, {3} -> {4}'.format(self.code, value1, value2, self.args[2], self.memory[self.args[2]]))
        return res

class Input(Operation):
    code = 3
    num_args = 1

    def __init__(self, memory, address, modes, inputs, relative_base):
        super(Input, self).__init__(memory, address, modes, inputs, relative_base)

    def execute(self):
        if not self.inputs:
            ip = int(input('Enter a value: '))
        else:
            ip = self.inputs.pop()
            # print("Auto Input: {0}".format(ip))
        self.write_arg(0, ip)
        return ip
    
class Output(Operation):
    code = 4
    num_args = 1

    def __init__(self, memory, address, modes, inputs, relative_base):
        super(Output, self).__init__(memory, address, modes, inputs, relative_base)

    def execute(self):
        arg = self.read_arg(0)
        # print('Output: {0}'.format(arg))
        return arg

class JumpIfTrue(Operation):
    code = 5
    num_args = 2

    def __init__(self, memory, address, modes, inputs, relative_base):
        super(JumpIfTrue, self).__init__(memory, address, modes, inputs, relative_base)

    def next_address(self):
        return self.address

    def execute(self):
        value1 = self.read_arg(0)
        value2 = self.read_arg(1)
        result = value2 if value1 >= 1 else self.address+self.num_args+1
        self.address = result
        return result

class JumpIfFalse(Operation):
    code = 6
    num_args = 2

    def __init__(self, memory, address, modes, inputs, relative_base):
        super(JumpIfFalse, self).__init__(memory, address, modes, inputs, relative_base)

    def next_address(self):
        return self.address

    def execute(self):
        value1 = self.read_arg(0)
        value2 = self.read_arg(1)
        result = value2 if value1 == 0 else self.address+self.num_args+1
        self.address = result
        return result

class LessThan(Operation):
    code = 7
    num_args = 3

    def __init__(self, memory, address, modes, inputs, relative_base):
        super(LessThan, self).__init__(memory, address, modes, inputs, relative_base)

    def execute(self):
        value1 = self.read_arg(0)
        value2 = self.read_arg(1)
        result = 1 if value1 < value2 else 0
        # self.memory[self.args[2]] = result
        self.write_arg(2, result)
        return result

class Equals(Operation):
    code = 8
    num_args = 3

    def __init__(self, memory, address, modes, inputs, relative_base):
        super(Equals, self).__init__(memory, address, modes, inputs, relative_base)

    def execute(self):
        value1 = self.read_arg(0)
        value2 = self.read_arg(1)
        result = 1 if value1 == value2 else 0
        # self.memory[self.args[2]] = result
        self.write_arg(2, result)
        return result

class SetRelativeBase(Operation):
    code = 9
    num_args = 1

    def __init__(self, memory, address, modes, inputs, relative_base):
        super(SetRelativeBase, self).__init__(memory, address, modes, inputs, relative_base)
    
    def execute(self):
        value = self.read_arg(0)
        print("Relative Base Offset: {0}".format(value))
        return self.relative_base + value

class Memory(object):
    def __init__(self, initial_Values=None):
        self.mem = initial_Values or []

    def __setitem__(self, x, y):
        print('setting mem at {0} to {1}'.format(x,y))
        print(len(self.mem))
        if len(self.mem) <=  x:
            self.mem.extend([0]*(x-(len(self.mem)-1)))
        self.mem[x] = y

    def __getitem__(self, x):
        if len(self.mem) >= x:
            return self.mem.__getitem__(x)
        return 0

    def __len__(self):
        return self.mem.__len__()

    def __repr__(self):
        return self.mem.__repr__()

class Computer:
    operations = {
        Add.code: Add,
        Multiply.code: Multiply,
        Input.code: Input,
        Output.code: Output,
        JumpIfTrue.code: JumpIfTrue,
        JumpIfFalse.code: JumpIfFalse,
        LessThan.code: LessThan,
        Equals.code: Equals,
        SetRelativeBase.code: SetRelativeBase
    }
    outputs = []
    halted = False
    relative_base = 0

    def __init__(self, memory, auto_inputs=None):
        self.memory = Memory(memory)
        self.pos = 0
        self.auto_inputs = auto_inputs or []
        self.auto_inputs.reverse()

    def compute(self):
        while True:
            # print('pos: {0}'.format(self.pos))
            opcode = list(str(self.memory[self.pos]))

            code = int(''.join(opcode[-2:]))
            modes = opcode[:-2]
            modes.reverse()
            
            if code == 99:
                # print('halting')
                self.halted = True
                break
            
            if code not in self.operations:
                raise ValueError('Code {0} is invalid'.format(code))

            op = self.operations[code](self.memory, self.pos, modes, self.auto_inputs, self.relative_base)
            self.outputs.append(op.execute())
            self.pos = op.next_address()

            if code == Output.code:
                # print('signalling')
                break
            elif code == SetRelativeBase.code:
                # print("Chainging Relative Base from {0} to {1}".format(self.relative_base, self.outputs[-1]))
                self.relative_base = self.outputs[-1]

        return self.memory

class Robot:
    direction = 0
    locationX = 0
    locationY = 0
    panels_painted = 0

    def __init__(self, memory):
        self.computer = Computer(memory)
        self.map = {}
    
    def _move(self):
        if self.direction == 0:
            self.locationY +=1
        elif self.direction == 90:
            self.locationX += 1
        elif self.direction == 180:
            self.locationY -= 1
        elif self.direction == 270:
            self.locationX -= 1
    
    def _turn(self, degrees):
        if self.direction == 270 and degrees == 90:
            self.direction = 0
        elif self.direction == 0 and degrees == -90:
            self.direction = 270
        else:
            self.direction += degrees
    
    def run(self):
        current_colour = 1
        while not self.computer.halted:
            self.computer.auto_inputs.insert(0, current_colour)
            self.computer.compute()
            new_colour = self.computer.outputs[-1]

            if current_colour != new_colour and (self.locationX,self.locationY) not in self.map:
                self.panels_painted +=1

            self.map[(self.locationX,self.locationY)] = new_colour

            self.computer.compute()
            self._turn(90 if self.computer.outputs[-1] == 1 else -90)
            self._move()
            current_colour = self.map.get((self.locationX,self.locationY), 0)

    def paint(self):
        minX = 0
        minY = 0
        maxX = 0
        maxY = 0
        
        for key in self.map.keys():
            if key[0] < minX:
                minX = key[0]

            if key[0] > maxX:
                maxX = key[0]
            
            if key[1] < minY:
                minY = key[1]

            if key[1] > maxY:
                maxY = key[1]
            
        print('MinX: {0}, MinY: {1}'.format(minX, minY))
        print('MaxX: {0}, MaxY: {1}'.format(maxX, maxY))

        for y in range(maxY, minY-1, -1):
            row = ''
            for x in range(minX, maxX+1):
                row += '1' if self.map.get((x,y),0) == 1 else ' '
            print(row)


def compute(memory, auto_inputs):
    computer = Computer(memory.copy(), auto_inputs)
    while not computer.halted:
        computer.compute()
    return computer.outputs[-1]

def program_alarm(memory, noun, verb):
    memory[1] = noun
    memory[2] = verb
    computer = Computer(memory.copy(), [])
    while not computer.halted:
        res = computer.compute()[0]
    return res

def run_sequence(memory, sequence):
    next_input = 0
    result = 0
    computers = []

    for phase_setting in sequence:
        computers.append(Computer(memory.copy(), [phase_setting]))

    while (all([not computer.halted for computer in computers])):  
        for i, computer in enumerate(computers):
            computer.auto_inputs.insert(0, next_input)
            print('Computer {0}: {1}'.format(i, computer.auto_inputs))
            computer.compute()
            next_input = computer.outputs[-1]
            result = next_input if not computer.halted else result

    return result

def run_day7(sequence):
    max_res = 0
    max_res_sequence = None
    
    for s in itertools.permutations(sequence):
        print('Sequence: {0}'.format(sequence))
        res = run_sequence(data.day7.copy(), s)
        if res > max_res:
            max_res = res
            max_res_sequence = s

    print("Sequence Producing Max Result of {0}: {1}".format(max_res, max_res_sequence))
    return max_res

def run_tests():
    # assert 65464 == run_day7([0,1,2,3,4])
    # assert 1518124 == run_day7([5,6,7,8,9])
    # assert 19690720 == program_alarm(data.day2.copy(), 49, 25)
    # assert 8332629 == compute(data.day5.copy(), [1])
    # assert 8805067 == compute(data.day5.copy(), [5])
    # assert 43210 == run_sequence([3,15,3,16,1002,16,10,16,1,16,15,15,4,15,99,0,0], [4,3,2,1,0])
    # assert 54321 == run_sequence([3,23,3,24,1002,24,10,24,1002,23,-1,23, 101,5,23,23,1,24,23,23,4,23,99,0,0], [0,1,2,3,4])
    # assert 65210 == run_sequence([3,31,3,32,1002,32,10,32,1001,31,-2,31,1007,31,0,33,1002,33,7,33,1,33,31,31,1,32,31,31,4,31,99,0,0,0], [1,0,4,3,2])
    # assert 139629729 == run_sequence([3,26,1001,26,-4,26,3,27,1002,27,2,27,1,27,26,27,4,27,1001,28,-1,28,1005,28,6,99,0,0,5], [9,8,7,6,5])
    # print(compute([109,1,204,-1,1001,100,1,100,1008,100,16,101,1006,101,0,99], []))
    assert 1219070632396864 == compute([1102,34915192,34915192,7,4,7,99,0], [])
    assert 1125899906842624 == compute([104,1125899906842624,99], [])
    # compute(data.day9, [])
    robot = Robot(data.day11.copy())
    robot.run()
    robot.paint()
    # print(robot.map)
    # print(robot.panels_painted)

def main():
    run_tests()
    # run([0,1,2,3,4])
    # run([5,6,7,8,9])

if __name__ == "__main__":
    main()