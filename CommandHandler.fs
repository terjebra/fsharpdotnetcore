module CommandHandler

open System
open Todo
open Events
open Commands
open Common

type GetEventsById =
   Id -> DomainEvent list

type SaveTodoEvent = 
  Id -> DomainEvent -> unit

type CommandResult = {
  state: TodoState
  events: DomainEvent list
}

let applyEvent log state event  =
  match event.eventType with
    | TodoCreated name ->
      Todo.create name event.id state log
    | TodoCompleted ->
      Todo.complete state log
    | TodoNameChanged name ->
      Todo.changeName name state log
    | TodoDeleted ->
      Todo.delete state log

let handleCommand log command state =
  let event = 
    match command.action with 
      | CreateTodo name ->  {id= command.id; eventType = TodoCreated(name) }
      | ChangeNameTodo name ->  {id= command.id; eventType = TodoNameChanged(name)}
      | CompleteTodo ->  {id= command.id; eventType = TodoCompleted}
      | DeleteTodo -> {id= command.id; eventType = TodoDeleted}
  
  let newState = applyEvent log state event

  {state = newState; events = [event]}

let commandHandler(log: Log) 
                  (getEvents: GetEventsById) 
                  (saveEvent: SaveTodoEvent)
                  (command: Command) =
  let previousEvents = getEvents command.id
  
  let currentState =
    let nolog = ignore
    previousEvents |> List.fold (applyEvent nolog) Todo.initialState 
  
  let result = handleCommand log command currentState

  let {events = pendingEvents; state = newState} = result

  pendingEvents |> List.iter (saveEvent command.id)

  newState


