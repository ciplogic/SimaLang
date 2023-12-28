//one method interface
interface Supplier[T]() T
interface Consumer[T](val:T)
interface Mapper[From, To](From):To
interface Runnable()

fn select[From, To](values: *From, mapper:Mapper[From,To]): *To {
    for it:values {
       yield *value
    }
} 
value Point (x: i32, y: i32) 

fn main():i32 {
    fn printIt () { 
      print ("Hello: ${x}");
    };
    x = 2.toStr()
    pt = Point(3,5)
    printIt()
}
