import data

class Operation:
    code = None
    num_args = 0

    def __init__(self, memory, address, modes):
        self.memory = memory
        self.address = address
        self.modes = modes
        self.args = []

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

    def __init__(self, memory, address, modes):
        super(Add, self).__init__(memory, address, modes)

    def execute(self):
        value1 = self.read_arg(0)
        value2 = self.read_arg(1)
        self.memory[self.args[2]] = self.read_arg(0) + self.read_arg(1)
        # print('{0}, {1}, {2}, {3} -> {4}'.format(self.code, value1, value2, self.args[2], self.memory[self.args[2]]))

class Multiply(Operation):
    code = 2
    num_args = 3

    def __init__(self, memory, address, modes):
        super(Multiply, self).__init__(memory, address, modes)
    
    def execute(self):
        value1 = self.read_arg(0)
        value2 = self.read_arg(1)
        self.memory[self.args[2]] = self.read_arg(0) * self.read_arg(1)
        # print('{0}, {1}, {2}, {3} -> {4}'.format(self.code, value1, value2, self.args[2], self.memory[self.args[2]]))

class Input(Operation):
    code = 3
    num_args = 1

    def __init__(self, memory, address, modes):
        super(Input, self).__init__(memory, address, modes)

    def execute(self):
        self.memory[self.args[0]] = int(input('Enter a value: '))
    
class Output(Operation):
    code = 4
    num_args = 1

    def __init__(self, memory, address, modes):
        super(Output, self).__init__(memory, address, modes)

    def execute(self):
        print('Output: {0}'.format(self.read_arg(0)))

class JumpIfTrue(Operation):
    code = 5
    num_args = 2

    def __init__(self, memory, address, modes):
        super(JumpIfTrue, self).__init__(memory, address, modes)

    def next_address(self):
        return self.address

    def execute(self):
        value1 = self.read_arg(0)
        value2 = self.read_arg(1)
        self.address = value2 if value1 >= 1 else self.address+self.num_args+1

class JumpIfFalse(Operation):
    code = 6
    num_args = 2

    def __init__(self, memory, address, modes):
        super(JumpIfFalse, self).__init__(memory, address, modes)

    def next_address(self):
        return self.address

    def execute(self):
        value1 = self.read_arg(0)
        value2 = self.read_arg(1)
        self.address = value2 if value1 == 0 else self.address+self.num_args+1

class LessThan(Operation):
    code = 7
    num_args = 3

    def __init__(self, memory, address, modes):
        super(LessThan, self).__init__(memory, address, modes)

    def execute(self):
        value1 = self.read_arg(0)
        value2 = self.read_arg(1)
        self.memory[self.args[2]] = 1 if value1 < value2 else 0

class Equals(Operation):
    code = 8
    num_args = 3

    def __init__(self, memory, address, modes):
        super(Equals, self).__init__(memory, address, modes)

    def execute(self):
        value1 = self.read_arg(0)
        value2 = self.read_arg(1)
        self.memory[self.args[2]] = 1 if value1 == value2 else 0

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

    def __init__(self, memory):
        self.memory = memory
        self.pos = 0

    def compute(self):
        while True:
            # print('pos: {0}'.format(self.pos))
            opcode = list(str(self.memory[self.pos]))

            code = int(''.join(opcode[-2:]))
            modes = opcode[:-2]
            modes.reverse()
            
            if code == 99:
                # print('99')
                break
            
            if code not in self.operations:
                raise ValueError('Code {0} is invalid'.format(code))

            op = self.operations[code](self.memory, self.pos, modes)
            op.execute()
            self.pos = op.next_address()
        return self.memory

def program_alarm(memory, noun, verb):
    memory[1] = noun
    memory[2] = verb
    computer = Computer(memory.copy())
    return computer.compute()[0]

def diagnostics(memory):
    pass

def gravity_calc():
    target = 19690720

    for noun in range(0,100):
        for verb in range(0,100):
            if program_alarm(data.input2.copy(), noun, verb) == target:
                print('Found target. Noun: {0}, Verb: {1}'.format(noun, verb))
                return 100*noun+verb

def compute(memory):
    computer = Computer(memory.copy())
    return computer.compute()

def run_test():
    compute(data.input.copy())

def main():
    # print (program_alarm(data.input.copy(), 12, 2))
    # print (gravity_calc())
    # print (compute([3,0,4,0,99]))
    # print(compute([1,9,10,3,2,3,11,0,99,30,40,50]))
    # print(compute([1002,4,3,4,33]))
    # print(data.input[244:247])
    run_test()
    # print(compute([1005,1,3,99]))
    # print(compute([5,1,3,4,99]))
    # print(compute([5,0,3,99]))
    # print(compute([6,0,3,4,99]))
    # print(compute([1006,0,3,99]))
    # print(compute([6,1,3,99]))

    # print(compute([3,9,8,9,10,9,4,9,99,-1,8]))  # 1 if input == 8, 0 if input != 8
    # print(compute([3,3,1108,-1,8,3,4,3,99]))


    # print(compute([3,9,7,9,10,9,4,9,99,-1,8]))  # 1 if input < 8, 0 if input >= 8
    # print(compute([3,3,1107,-1,8,3,4,3,99]))

    # print(compute([3,12,6,12,15,1,13,14,13,4,13,99,-1,0,1,9]))
    # print(compute([3,3,1105,-1,9,1101,0,0,12,4,12,99,1]))

    # print(compute([
    #     3,21,
    #     1008,21,8,20,
    #     1005,20,22,
    #     107,8,21,20,
    #     1006,20,31,
    #     1106,0,36,
    #     98,0,0,
    #     1002,21,125,20,
    #     4,20,
    #     1105,1,46,
    #     104,999,1105,1,46,1101,1000,1,20,4,20,1105,1,46,98,99]))


if __name__ == "__main__":
    main()