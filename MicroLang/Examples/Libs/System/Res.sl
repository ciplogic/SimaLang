value Res<T>(Value:T, Error:String)
value& String(Data: []i8)
fn Len(this: String) : int32 {
   return this.Data.Len-1;
}
