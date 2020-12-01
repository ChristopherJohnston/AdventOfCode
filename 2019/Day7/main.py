import data
import itertools

class Operation:
    code = None
    num_args = 0
    inputs = None

    def __init__(self, memory, address, modes, inputs):
        self.memory = memory
        self.address = address
        self.modes = modes
        self.args = []
        self.inputs = inputs

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
        return self.memory[self.args[i]] if self.modes[i] == '0' else self.args[i]

class Add(Operation):
    code = 1
    num_args = 3

    def __init__(self, memory, address, modes, inputs):
        super(Add, self).__init__(memory, address, modes, inputs)

    def execute(self):
        value1 = self.read_arg(0)
        value2 = self.read_arg(1)
        res = value1 + value2
        self.memory[self.args[2]] = res
        # print('{0}, {1}, {2}, {3} -> {4}'.format(self.code, value1, value2, self.args[2], self.memory[self.args[2]]))
        return res

class Multiply(Operation):
    code = 2
    num_args = 3

    def __init__(self, memory, address, modes, inputs):
        super(Multiply, self).__init__(memory, address, modes, inputs)
    
    def execute(self):
        value1 = self.read_arg(0)
        value2 = self.read_arg(1)
        res = value1 * value2
        self.memory[self.args[2]] = res
        # print('{0}, {1}, {2}, {3} -> {4}'.format(self.code, value1, value2, self.args[2], self.memory[self.args[2]]))
        return res

class Input(Operation):
    code = 3
    num_args = 1

    def __init__(self, memory, address, modes, inputs):
        super(Input, self).__init__(memory, address, modes, inputs)

    def execute(self):
        if not self.inputs:
            ip = int(input('Enter a value: '))
        else:
            ip = self.inputs.pop()
            print("Auto Input: {0}".format(ip))
        self.memory[self.args[0]] = ip
        return ip
    
class Output(Operation):
    code = 4
    num_args = 1

    def __init__(self, memory, address, modes, inputs):
        super(Output, self).__init__(memory, address, modes, inputs)

    def execute(self):
        arg = self.read_arg(0)
        print('Output: {0}'.format(arg))
        return arg

class JumpIfTrue(Operation):
    code = 5
    num_args = 2

    def __init__(self, memory, address, modes, inputs):
        super(JumpIfTrue, self).__init__(memory, address, modes, inputs)

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

    def __init__(self, memory, address, modes, inputs):
        super(JumpIfFalse, self).__init__(memory, address, modes, inputs)

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

    def __init__(self, memory, address, modes, inputs):
        super(LessThan, self).__init__(memory, address, modes, inputs)

    def execute(self):
        value1 = self.read_arg(0)
        value2 = self.read_arg(1)
        result = 1 if value1 < value2 else 0
        self.memory[self.args[2]] = result
        return result

class Equals(Operation):
    code = 8
    num_args = 3

    def __init__(self, memory, address, modes, inputs):
        super(Equals, self).__init__(memory, address, modes, inputs)

    def execute(self):
        value1 = self.read_arg(0)
        value2 = self.read_arg(1)
        result = 1 if value1 == value2 else 0
        self.memory[self.args[2]] = result
        return result

class Computer:
    operations = {
        Add.code: Add,
        Multiply.code: Multiply,
        Input.code: Input,
        Output.code: Output,
        JumpIfTrue.code: JumpIfTrue,
        JumpIfFalse.code: JumpIfFalse,
        LessThan.code: LessThan,
        Equals.code: Equals
    }
    outputs = []
    halted = False

    def __init__(self, memory, auto_inputs=None):
        self.memory = memory
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
                print('halting')
                self.halted = True
                break
            
            if code not in self.operations:
                raise ValueError('Code {0} is invalid'.format(code))

            op = self.operations[code](self.memory, self.pos, modes, self.auto_inputs)
            self.outputs.append(op.execute())
            self.pos = op.next_address()

            if code == Output.code:
                print('signalling')
                break
        return self.memory

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

def run(sequence):
    max_res = 0
    max_res_sequence = None
    
    for s in itertools.permutations(sequence):
        print('Sequence: {0}'.format(sequence))
        res = run_sequence(data.input.copy(), s)
        if res > max_res:
            max_res = res
            max_res_sequence = s

    print("Sequence Producing Max Result of {0}: {1}".format(max_res, max_res_sequence))

def run_tests():
    assert 19690720 == program_alarm(data.day2.copy(), 49, 25)
    assert 8332629 == compute(data.day5.copy(), [1])
    assert 8805067 == compute(data.day5.copy(), [5])
    assert 43210 == run_sequence([3,15,3,16,1002,16,10,16,1,16,15,15,4,15,99,0,0], [4,3,2,1,0])
    assert 54321 == run_sequence([3,23,3,24,1002,24,10,24,1002,23,-1,23, 101,5,23,23,1,24,23,23,4,23,99,0,0], [0,1,2,3,4])
    assert 65210 == run_sequence([3,31,3,32,1002,32,10,32,1001,31,-2,31,1007,31,0,33,1002,33,7,33,1,33,31,31,1,32,31,31,4,31,99,0,0,0], [1,0,4,3,2])
    assert 139629729 == run_sequence([3,26,1001,26,-4,26,3,27,1002,27,2,27,1,27,26,27,4,27,1001,28,-1,28,1005,28,6,99,0,0,5], [9,8,7,6,5])

def main():
    run_tests()
    # run([0,1,2,3,4])
    # run([5,6,7,8,9])

if __name__ == "__main__":
    main()