import data

class Node:
    def __init__(self, name):
        self.name = name
        self.root = True
        self.children = []

def main():
    m = {}

    for orbit in data.input:
        l, r = orbit.split(')')
        if l not in m:
            m[l] = Node(l)
        if r not in m:
            m[r] = Node(r)
        m[r].root = False
        m[l].children.append(m[r])
        m[r].children.append(m[l])

    seen = set()
    def _traverse(node, depth=0):
        if node.name in seen:
            print(seen)
            return 0
        seen.add(node.name)
        return depth + sum(_traverse(child, depth+1) for child in node.children)

    val = sum(_traverse(node) for node in m.values() if node.root == True)

    print(val)


if __name__ == "__main__":
    main()