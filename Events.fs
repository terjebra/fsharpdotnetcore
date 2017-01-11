module Events
open Common

type EventType =
  | TodoCreated of Name: string
  | TodoCompleted
  | TodoNameChanged of Name: string
  | TodoDeleted 


type DomainEvent = {
  id: Id
  eventType: EventType
}