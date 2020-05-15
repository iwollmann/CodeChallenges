const fs = require('fs')

let Node = function () {
    this.keys = new Map();
    this.end = false;
    this.idxAndValue = [];

    this.setIdxAndValue = (iv) => {
        this.idxAndValue.push(iv);
    }

    this.setEnd = () => {
        this.end = true;
    }

    this.isEnd = () => {
        return this.end;
    }
}

let Trie = function () {
    this.root = new Node();

    this.add = (input, iv, node = this.root) => {
        if (input.length === 0) {
            node.setIdxAndValue(iv);
            node.setEnd();
            return;
        }

        if (!node.keys.has(input[0])) {
            node.keys.set(input[0], new Node());
        }

        return this.add(input.substr(1), iv, node.keys.get(input[0]));
    }

    this.findNode = (word) => {
        let node = this.root;
        while (word.length > 1) {
            if (!node.keys.has(word[0])) {
                return undefined; //todo return index
            } else {
                node = node.keys.get(word[0]);
                word = word.substr(1);
            }
        }

        return (node.keys.has(word)) ? node.keys.get(word) : undefined;
    }
}

const Main = (trie, dna) => {
    const crrDna = dna.split(" "); //"1 5 caaab".split(" ");
    const first = crrDna[0], last = crrDna[1];
    dna = crrDna[2];
    let current = '';
    let count = 0;

    for (let index = 0; index < dna.length; index++) {
        current = '';
        let letter = 0;
        while (true) {
            current = current + dna[letter + index];
            const node = trie.findNode(current);

            if (!node) {
                break;
            } else {
                if (node.end) {
                    for (let j = 0; j < node.idxAndValue.length; j++) {
                        if (node.idxAndValue[j][0] >= first && node.idxAndValue[j][0] <= last) {
                            count += node.idxAndValue[j][1];
                        }
                    }
                }
                letter++;
            }
        }
    }

    return count;
}

try {
    const [input, expected] = process.argv.slice(2);

    const startHrTime = process.hrtime();
    const expectedData = fs.readFileSync(expected, 'utf8')
    const data = fs.readFileSync(input, 'utf8')

    const [n, genesLine, healthLine, s, ...dna] = data.split("\n");
    let genes = genesLine.split(" ");
    let health = healthLine.split(" ");

    let trie = new Trie();
    for (let index = 0; index < n; index++) {
        trie.add(genes[index], [index, Number.parseInt(health[index])]);
    }

    let max = 0;
    let min = Number.MAX_SAFE_INTEGER;
    for (let index = 0; index < dna.length; index++) {
        let result = Main(trie, dna[index]);

        if (result > max) {
            max = result;
        }

        if (result < min) {
            min = result;
        }
    }

    const [minEx, maxEx] = expectedData.split(' ');
    console.log(`Result: ${min} ${max}`);
    console.log(`Expected: ${minEx} ${maxEx}`);
    // console.log(min === Number.parseInt(minEx) && max === Number.parseInt(maxEx) ? 'TRUE' : 'FALSE');

    const elapsedHrTime = process.hrtime(startHrTime);
    const elapsedTimeInMs = elapsedHrTime[0] * 1000 + elapsedHrTime[1] / 1e6;
    console.log(elapsedTimeInMs);

} catch (err) {
    console.error(err)
}
