module Storage
open System
open Events

type EventStore() = 
    let events = System.Collections.Generic.Dictionary<System.Guid, DomainEvent list>()
    
    let publishEvent = new Event<System.Guid * DomainEvent>()

    member this.SaveEvent = 
     publishEvent.Publish 
    
    member this.Save(eventId, event) = 
      match events.TryGetValue eventId with
      | true, eventList -> 
          let newList = event :: eventList
          events.[eventId] <- newList 
      | false, _ -> 
          let newList = [event]
          events.[eventId] <- newList 
      publishEvent.Trigger(eventId,event)
    
     member this.Get eventId = 
        match events.TryGetValue eventId with
        | true,eventList -> 
            eventList 
            |> Seq.toList  
            |> List.rev  
        | false, _ -> 
            []

