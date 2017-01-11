module Todo

open System

type TodoState = {
  id: System.Guid
  name: string
  deleted: bool
  completed: bool
}

let initialState = {
  id = System.Guid.Empty
  name = ""
  deleted = false
  completed = false
} 

let create name id state log = 
  log (sprintf "Created with id: %A and name: %s "id name)
  {state with name = name; id = id}
  
let changeName name state log = 
  log (sprintf "Name changed on todo with #id: %A -> name: %s " state.id  name)
  {state with name = name}

let delete state log = 
  log (sprintf "Deleted todo with #id: %A" state.id)
  {state with deleted = true}

let complete state log = 
  log (sprintf "Completed todo with #id: %A" state.id)
  {state with completed = true}

