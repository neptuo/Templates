Templates
=========

Base framework for creating UI templates that are compiled for running both on server-side and client-side (in javascript).

The framework is composed from three projects:

**Neptuo.Templates.Compilation** is base project for processing, compiling and executing templates.

**Neptuo.Templates.Components** is one of possible implementations for the structure of compiled template. In these implementation every xml element is compiled to registered class and this class is instantiated by the framework.

**Neptuo.Templates.Components.Compilation** is backing project for support of *Neptuo.Templates.Components*.
