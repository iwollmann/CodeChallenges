// Having a list of n digits (0 <= digit <= 9), where n less than or equal to 100, arrange it to move all 0 to right in O (n) time.

// Example [4, 8, 0, 9, 2, 5, 0, 3, 3, 0] -> [4, 8, 9, 2, 5, 3, 3, 0, 0, 0]

// You must display this list in the console.

// After this, the previous list without zeros ([4, 8, 9, 2, 5, 3, 3]) will represent a fictitious integer (4892533).
//  You should add 1 unit to it, leaving the final list as [4, 8, 9, 2, 5, 3, 4]. You must display this list in the console.

// Finally, multiply by -1 each digit in even position of the array without zeros. 
// After this, detect the sub-array, whose sum of digits is the maximum and return this sum. 
// Example: [4, 8, 9, 2, 5, 8, 4, 9] -> [-4, 8, -9, 2, -5, 8, -4, 9] -> 13. You must display the sum in the console.

function shiftZeros(arr) {
    let zeroIndex = 0;
    let aux;

    // [4, 8, 0, 9, 2, 5, 0, 3, 1, 0]
    for (let index = 0; index < arr.length; index++)
    {
        if (arr[index] != 0) 
        {
            aux = arr[zeroIndex];
            arr[zeroIndex] = arr[index];
            arr[index] = aux;
            zeroIndex += 1;
        }
    }

    console.log(arr);

    return [arr, zeroIndex];
}

function add1ToArray(arr) {
    const n = arr.length;
    let newArr = arr;
    let add = true;
    let carry = 0;
    for (let index = n-1; index >= 0; index--) {
        if (add) {
            if (arr[index]+1 === 10) {
                arr[index] = 0;
                if (index === 0) carry++;
            } else {
                arr[index] = arr[index]+1;
                add = false;
            }
        } else {
            arr[index] = arr[index];
            add = false;
        }
    }

    if (carry === 1) {
        newArr = new Array(arr.length +1);
        newArr = [1, ...arr];
    }

    console.log(newArr);

    return newArr;
}

function findMaxSubArray(arr) {
    // [-4, 8, -9, 2, -5, 8, -4, 9]
    let maxSum = 0;
    let currentSum = 0;
    // let initialIndex = 0;
    // let finalIndex = 0;
    for (let index = 0; index < arr.length; index++) {
        currentSum += arr[index];

        if (currentSum < 0) {
            // initialIndex = index+1;
            currentSum = 0;
        }

        if (maxSum < currentSum) {
            // finalIndex = index;
            maxSum = currentSum;
        }
    }

    // console.log(maxSum, initialIndex, finalIndex, arr.splice(initialIndex, finalIndex));
    console.log(maxSum);
}


// shiftZeros([4, 8, 0, 9, 2, 5, 0, 3, 1, 0]);
const [arr, index] = shiftZeros([4, 8, 9, 2, 5, 8, 4, 8]);
const onlyValuesArray = arr.splice(0, index);
console.log(onlyValuesArray);
add1ToArray([9,9,9]);
let plus1Array = add1ToArray(onlyValuesArray);

plus1Array = plus1Array.map((v, i)=> (i % 2 === 0) ? v*-1 : v);  // multiply by -1 each digit in even position
console.log(plus1Array);

findMaxSubArray(plus1Array);

