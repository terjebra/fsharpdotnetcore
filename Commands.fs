module Commands
open Common

type CommandAction = 
  | CreateTodo of Name: string
  | ChangeNameTodo of Name: string
  | CompleteTodo
  | DeleteTodo

type Command = {
  id: Id
  action: CommandAction 
}
