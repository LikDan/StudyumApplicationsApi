Studyum Applications

**Parser**

### Variables

#### Input data

``` json
{
  "primitive": 1,
  "object": {
    "key": "value"
  },
  "array": [2],
  "objectsArray": [
    {
      "a": "b"
    }
  ]
}
```

#### Input template

```html

<div>
    {ctx.input.primitive}
    {ctx.input.object}
    {ctx.input.object.key}
    {ctx.input.array}
    {ctx.input.array[0]}
    {ctx.input.objectsArray}
    {ctx.input.objectsArray[0]}
    {ctx.input.objectsArray[0].a}

    {ctx.user}
    {ctx.studyPlace}
    
    {ctx.time}
    {ctx.date}
    {ctx.datetime}
    
    {ctx.env}
</div>
```

#### Output

```html

<div>
    1
    {"key": "value"}
    value
    [1]
    2
    [{"a": "b"}]
    {"a": "b"}
    b
    
    {"name": "USERNAME", "group": "USER_GROUP"...}
    {"name": "STUDY_PLACE_NAME"...}
    
    CURRENT_TIME
    CURRENT_DATE
    CURRENT_DATETIME
    
    {"id": "APPLICATION_TEMPLATE_ID", "name": "APPLICATION_NAME"...}
</div>
```

### Conditions

#### Input data

``` json
{
  "false": false,
  "true": true,
  "number": 10,
  "string": "1",
  "nested": {
    "value": 10
  }
}
```

#### Input template

```html

<div>
    <if condition="{ctx.input.false}">
        <p>False will not be in final result</p>
    </if>
    <if condition="{ctx.input.true}">
        <p>True will be in final result</p>
    </if>
    <if condition="{ctx.input.number}">
        <p>Number not 0 will be in final result</p>
    </if>
    <if condition="{ctx.input.number} == 10">
        <p>Number will be in final result</p>
    </if>
    <if condition="{ctx.input.string} == 1">
        <p>String as Int will not be in final result</p>
    </if>
    <if condition="{ctx.input.string} == 1">
    <p>String will be in final result</p>
    </if>
    <if condition="{ctx.input.nested.value} == 10">
        <p>Nested will be in final result</p>
    </if>

    <if condition="{ctx.input.nested.value} == 9">
        <p>Value eq 9</p>
        <else>
            <p>Value not eq 9</p>
        </else>
    </if>

    <if condition="{ctx.input.nested.value} == 9">
        <p>Value eq 9</p>
        <if-else condition="{ctx.input.nested.value} == 10">
            <p>Value eq 10</p>
        </if-else>
        <else>
            <p>Value not eq 9 or 10</p>
        </else>
    </if>
</div>
```

#### Output

```html

<div>
    <p>True will be in final result</p>
    <p>Number not 0 will be in final result</p>
    <p>Number will be in final result</p>
    <p>String will be in final result</p>
    <p>Nested will be in final result</p>
    <p>Value not eq 9</p>
    <p>Value eq 10</p>
</div>
```

### Loops

#### Input data

``` json
{
  "array": [
    {
      "title": "Title1",
      "number": 100,
      "nested": [
        {
          "value": 1
        },
        {
          "value": 2
        }
      ]
    },
    {
      "title": "Title2",
      "number": 200,
      "nested": [
        {
          "value": 10
        },
        {
          "value": 20
        }
      ]
    }
  ]
}
```

#### Input template

```html

<div>
    <loop index="i" value="value" array="ctx.input.array">
        <p>{i} {value.title} {value.number}</p>
        <div>
            <loop value="val" array="value.nested">
                <p>Nested: {val.value}</p>
            </loop>
        </div>
    </loop>
</div>
```

#### Output

```html

<div>
    <p>0 Title1 100</p>
    <div>
        <p>Nested: 1</p>
        <p>Nested: 2</p>
    </div>
    <p>1 Title2 200</p>
    <div>
        <p>Nested: 10</p>
        <p>Nested: 20</p>
    </div>
</div>
```