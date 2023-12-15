﻿//one method interface
interface Supplier<T>() T
interface Consumer<T>(val:T)
interface Mapper<From, To>(From):To
interface Runnable()

fn select<From, To>(values: ...From, mapper:Mapper<From,To>): ...To {
    for values.next() {
       yield *value
    }
} 
value Point (x: int32, y: int32) 

fn main():int32 {
    x = 2.toStr()
    pt = Point(3,5)
    fn printIt (i) { 
      print ("Hello: ${x}");
    };
    printIt(x)
}