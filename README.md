# Recognizers.Text.Wrapper

This library is a wrapper around the Microsoft.Recognizers.Text Library
allowing you to work with statically typed objects while still leveraging the
power of the Microsoft Recognizers. The goal is at some point to wrap all
of the Recognizers and put each Recognizer into it's own Project, allowing 
users to only pull in the dependencies they require. A subgoal of this is to 
allow users to remain in control of the way that they represent each wrapper 
object that is used. This means, for example, that we provide factory 
interfaces and abstract classes that can be utilized to define your own parse 
method for each of the types that we support.

## Supported Recognizers

| **Recognizer** | Supported          | NuGet Link                                                        |
|----------------|--------------------|-------------------------------------------------------------------|
| Number         | :x:                | -                                                                 |
| NumberWithUnit | :x:                | -                                                                 |
| DateTime       | :heavy_check_mark: | https://www.nuget.org/packages/Recognizers.Text.DateTime.Wrapper/ |
| Sequence       | :x:                | -                                                                 |
| Choice         | :x:                | -                                                                 |

## Examples

### DateTime

To get all possible `DateTimeV2Model` from some input, you can do the following
```c#
IEnumerable<DateTimeV2Model> models = DateTimeV2Recognizer
            .RecognizeDateTimes("I will be leaving at 12:15 and then come back before 2:30", 
                refTime: DateTime.Parse("2021-10-10T10:00:00"))
```

Now that you have the `models`, you can interact with each one to extract possible 
resolutions of the parsed DateTimes found by the Microsoft.Recognizers.Text.DateTime
library. 

For example, the above input will produce 2 models. The first model will be about `at 12:15`
and will mark it as a Time and produce a `DateTimeV2Time` for the models first Value. The 
second model will be about `before 2:30` and will be a `DateTimeV2TimeRange` for the models
first value and second value. However, each model doesn't specify whether or not you are
working with a `DateTimeV2Time` or `DateTimeV2TimeRange`, instead, they specify their type
with a Type enum and then you can cast the values to the appropriate type
for the factory in use. Since by default the BCL factory is in use, we get the types
specified above. 

For more help, please [start a discussion](https://github.com/Handy-Hangout-Studios/Recognizers.Text.Wrapper/discussions/new)

For any problems with the library or feature requests - [Create an issue](https://github.com/Handy-Hangout-Studios/Recognizers.Text.Wrapper/issues/new)
