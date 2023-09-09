# CPG_test_inter
**by Jarosław Krawczyszyn**

- Project was created in Unity 2022.3.7f1.
- Grid size is set in Assets/Resources/BoardConfig.json
- Other configurations as ScriptableObjects are in Assets/Configs
- Considering that only readability and simplicity of code would be evaluated, without mentioning extendability, I opted to use simple enum types to identify objects in board model. To better accomodate further project development I would use scriptable objects to easly add new types of fields and items, without developers involvement, and create gameplay model from this to use in gamestate.
- Adhering to not using third party packages I created simple DI manager. I usually use Zenject.
- Tested on a phone with 2500x2500 size and 120fps with no framerate drops.
