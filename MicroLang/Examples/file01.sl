//one method interface


fn select[From, To](values: *From, mapper:Mapper[From,To]): *To {
    for it:values {
        yield mapper(it)
    }
} 
value Point (x: i32, y: i32) 

fn main():i32 {
    fn printIt () { 
      print ("Hello: ${x}")
    }
    x = 2.toStr()
    pt = Point(3,5)
    printIt()
}
