# CustomVisionTrainer

Project helping upload images and train model with Azure Custom Vision service

## The way it works

Program takes folder structure from root directory that you provide, upload images from those folders, and then trains ML model from those images on Azure Cognitive Services.

### Tags creation

Lets imagine we had the following folder structure:

```bash

G:/Photos
|--- tree
|    |--- oak
|    |--- chestnut
|    |--- coniferous
|    |    |--- pine
|    |    |--- spruce

```

In every folder there is at least one photo of that tree spiece.

The tags that program will create:

* tree_oak
* tree_chestnut
* tree_coniferous
* tree_coniferous_pine
* tree_coniferous_spruce

You got the point :)

## Setup

Before you use this tool you need to adjust at least two from those settings:

```csharp
// From where folder structure is taken
private const string RootPath = @"G:\Photos";

// Azure Cognitive services training key
private const string TrainingKey = "";

// Cognitive services endpoint. You need to change only location
// (this endpoint is located in West Europe Azure Region)
private const string CustomVisionEndpoint = "https://westeurope.api.cognitive.microsoft.com/";
```
