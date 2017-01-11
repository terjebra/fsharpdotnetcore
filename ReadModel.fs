module ReadModel

type Todo = {
  id: System.Guid
  name: string
  completed: bool
  deleted: bool
}

type TodoReadModel() = 
    let todos = System.Collections.Generic.Dictionary<System.Guid, Todo>()
     
    member this.Save todo = 
      todos.[todo.id] <- todo

    member this.Get id = 
      match todos.TryGetValue id with
      | true, todo -> 
          Some todo  
      | false, _ -> 
          None
    
    member this.GetAll = 
      todos.Values |> Seq.toList  