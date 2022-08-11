function indexAnagram(s1, s2) {

  const s2Arr = s2.split("").sort()

  for (let i = 0; i < s1.length - s2.length + 1; i++) {
    
    let interval = ""
    
    try{
      interval = s1.substring(i, i + s2.length);
    } catch {
      break;
    }

    const intervalArr = interval.split("").sort() 

    console.log("\n------\n");
    console.log("i " + i)
    console.log("s1 " + s1)
    console.log("inter " + interval)
    console.log("s2 " + s2)
    console.log("interArr " + intervalArr)
    console.log("s2Arr " + s2Arr)

    if(intervalArr.join() == s2Arr.join()) return i;
  }
  return -1;
}

function testIndexAnagramSucesstestCase() {
  console.log(indexAnagram("actor", "cat"));
  console.log("\n--------------------\n");
  console.log(indexAnagram("actor", "tar"));
  console.log("\n--------------------\n");
  console.log(indexAnagram("actor", "rot"));
  console.log("\n--------------------\n");
}

testIndexAnagramSucesstestCase();