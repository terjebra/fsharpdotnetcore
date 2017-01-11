module EventProcessor
open System
open ReadModel
open Events
open Common

type GetTodoById =
   Id -> Option<Todo>

type SaveTodo = 
  Todo -> unit

let updateTodo (event: DomainEvent) (getTodo: GetTodoById) =
    match event.eventType with
    | TodoCreated name ->
      Some ({id = event.id; name = name; completed = false; deleted = false})
    | TodoCompleted ->
      let todo = getTodo event.id
      match todo with
        | Some existingTodo ->
            Some ({existingTodo with completed = true})
        | None -> None
    | TodoNameChanged name ->
      let todo = getTodo event.id          
      match todo with
        | Some existingTodo ->
            Some ({existingTodo with name = name})
        | None -> None
    | TodoDeleted ->
      let todo = getTodo event.id          
      match todo with
        | Some existingTodo ->
            Some ({existingTodo with deleted = true})
        | None -> None

let handleEventReceived (log: Log) 
                        (event: DomainEvent) 
                        (getTodo: GetTodoById)
                        (saveTodo: SaveTodo) =
  let todo = updateTodo event getTodo
  match todo with
    | Some updatedTodo ->
        saveTodo updatedTodo
    | None -> log (sprintf "Could not find todo with id# %A" event.id) 

let eventProcessor (log: Log) 
                  (eventStream: IObservable<Guid*DomainEvent>)  
                  (getTodo: GetTodoById)
                  (saveTodo: SaveTodo) =
  eventStream
  |> Observable.choose (function (id,ev) -> Some ev)
  |> Observable.subscribe (fun event -> handleEventReceived log event getTodo saveTodo)