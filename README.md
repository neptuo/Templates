Templates
=========

Base framework for creating UI templates that are compiled for running both on server-side and client-side (in javascript).

Example template can be found in [Index.html](test/Test.Templates.Console/Index.html). Not prefixed html tags are outputed as they are, those with prefix are instantiated and processess dynamicly. Beside html tags, also syntax in form {Binding Abc} is processed dynamicly. And special meaning has also prefixed html attributes, which are processed as extensions (or observers) on the original html tag.

More details about template processing can be found on our [WIKI](https://github.com/neptuo/Templates/wiki)
