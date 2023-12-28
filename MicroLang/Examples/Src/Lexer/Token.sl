enum TokenKind(
    None, 
    Spaces,
    Eoln, 
    Comment, 
    ReservedWord, 
    Identifier, 
    Operator, 
    Number, 
    String
)

value Token(text:Str, kind: TokenKind)
