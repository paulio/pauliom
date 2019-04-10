# pauliom
Coding samples

# LUISTools
The first of the LUIS Tools is the LuisToolsConsole. It's crazy complicated at the moment, it really needs to be rationalized but I'm putting it out there cause it does work, and hey perhaps you want to help change it?
## Generate LUIS Test Batches
[Batch testing with a set of example utterances](https://docs.microsoft.com/en-us/azure/cognitive-services/luis/luis-how-to-batch-test) is great way to test you LUIS endpoint. However, the batch format itself is quite difficult to construct, e.g. you have to enter the start and end positions of entities. This is where the LuisToolsConsole *GenerateExample* and *GenerateBatchTest* commands can help. They take a series of templates and generate the batch for you.

### Example Generator Map
```json
[
  {
    "Intent": "WhatIsTheirAge",
    "EntitySources": [
      {
        "EntityName": "personName",
        "FilePathToValue": "PersonNames.txt"
      }
    ]
  }
]
```
In the above example we are declaring an Intent that wish to test, *WhatIsTheirAge* and we're saying that we are going to populate the *personName* entity with values, per line, from the *PersonNames.txt* file.

### <entity file> (e.g. PersonNames.txt)
```json
Reginald Barclay
Bareil Antos
Julian Bashir  
```

The output of this first tool is an *ExamplesFile*

### Examples File
```json
{
  "Examples": [
    {
      "Intent": "WhatIsTheirAge",
      "ExamplesByEntity": [
        { "personName": "Reginald Barclay" },
        { "personName": "Bareil Antos" },
        { "personName": "Julian Bashir" }
      ]
    }
  ]
}
```
This can then be edited on it's own, but the real use is to then run it through a *TemplateFile* to generate the different test batch utterances

### Template File
```json
{
  "BatchTestItems": [
    {
      "text": "how old is <personName>",
      "intent": "WhatIsTheirAge",
      "entities": [
        {
          "entity": "personName"
        }
      ]
    },
    {
      "text": "what age is <personName>",
      "intent": "WhatIsTheirAge",
      "entities": [
        {
          "entity": "personName"
        }
      ]
    }
  ]
}
```

The *ExamplesFile* is then fed through the *TemplateFile* to finally produce the LUIS Batch File
```json
[
  {
    "text": "how old is Reginald Barclay",
    "intent": "WhatIsTheirAge",
    "entities": [
      {
        "entity": "personName",
        "startPos": 11,
        "endPos": 27
      }
    ]
  },
  {
    "text": "what age is Reginald Barclay",
    "intent": "WhatIsTheirAge",
    "entities": [
      {
        "entity": "personName",
        "startPos": 12,
        "endPos": 28
      }
    ]
  },
  {
    "text": "how old is Bareil Antos",
    "intent": "WhatIsTheirAge",
    "entities": [
      {
        "entity": "personName",
        "startPos": 11,
        "endPos": 23
      }
    ]
  },
  {
    "text": "what age is Bareil Antos",
    ... <ETC>
]
```
