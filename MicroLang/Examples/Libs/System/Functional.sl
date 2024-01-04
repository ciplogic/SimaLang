interface Supplier[T]() T
interface Consumer[T](val:T)
interface Mapper[From, To](val: From):To
interface Runnable()
interface Predicate[T](val: From): bool
