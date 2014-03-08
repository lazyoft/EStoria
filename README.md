EStoria
=======

A very simple and lightweight event sourcing framework.

***Why another Event Store?***
EStoria is a strongly opinionated event store with simplicity of use in mind. 
Despite all other great solutions around, EStoria is aimed at small to medium projects that want to experiment with this technology, without having to bother too much about the purity of the architecture. 
EStoria will trade off purity for ease of use, therefore some of the solutions and techniques adopted might not be "by the book".

The main features of EStoria are:

- Integrated durable store using simple file based persistence (with SQL Server support in the near future)
- Events are not bound to base classes or interfaces
- Simple global serial versioning of events
- Fluent and simple interface for defining projections and aggregates
- Observable pattern is used as a logical bus, thus simplifying the overall wiring of components
