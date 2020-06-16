# utility-ai
Система управления поведением неигровых персонажей (игровой искусственный интеллект) на основе принципа полезности для Unity. 

Система разрабатывалась в качестве выпускной квалификационной работы магистра.

Полная история коммитов и пример внедрения системы расположен в репозитории https://github.com/AnnRyazanova/survival-simulator

# Инструкция по использованию

Собранный Unity-пакет находится во вкладке release. Необходимо загрузить его и выполнить импорт в текущий проект Unity (Assets -> Import Package -> Custom Package -> выбрать ai-utility.unitypackage).

Файлы исходного кода появятся у вас в дирректории Assets/Scripts.

Для функционирования системы необходимо реализовать абстрактный класс контекста для каждого типа агентов. Контекст должен содержать вещественные параметры для оценщиков (допустимы последовательности вещественных в случае действий-селекторов) и параметры прочих типов, необходимых для выполнения действия. Необходимо так же занести идентификаторы в перечислимое ```AiContextVariable ```.

## Пример

```
public class AgentContext : AiContext
{
    void Start() => owner = GetComponent<AgentBehaviour>();

    public float Fullness => (owner as AgentBehaviour).Fullness;
    public float Boredom => (owner as AgentBehaviour).Boredom;
    public float Money => (owner as AgentBehaviour).Money;

    public override object GetParameter(AiContextVariable param){
        switch (param) {
            case AiContextVariable.Agent_Boredom:
                return Boredom;
            case AiContextVariable.Agent_Fullness:
                return Fullness;
            case AiContextVariable.Agent_Money:
                return Money;
            default:
                throw new ArgumentException();
        }
    }
}

...

public enum AiContextVariable   // Return type
{
    None,                       // : default switch branch
    Target,                     // : GameObject
    Owner,                       // : IAgent
    Agent_Fullness,
    Agent_Boredom,
    Agent_Money
}
```

Данный класс наследует логику ``` MonoBehaviour```, его необходимо прикрепить к префабу агента. 

Затем необходимо прикрепить к префабу компонент ``` NpcIntellect ```. В нем настроить переменную ``` Updates p/s``` для задания количества желаемых обновлений в секунду.

С помощью контекстного меню Unity путем навигации Utility_AI/Action или Utility_AI/Picker создать .asset файл действия либо действия селектора соответственно.   

Провести настройку действия путем добавления/удаления условий в соответствующем списке. Каждое условие принимает параметр, нормировочный сегмент, который задает диапазон **интересующих значений**, и оценочную функцию в виде анимационной кривой.

Квалификаторы выполняют агрегацию оценок всех условий. На выбор (по желанию) есть произведение оценок и усреднение.

Затем, в ранее созданном объекте  ``` NpcIntellect ```, добавить пустые действия в список  ``` Actions ``` и перетащить туда необходимые .asset файлы, созданные на предыдущем шаге.  

Для определения логики выполнения действий, необходимо в скрипте персонажа прописать методы каждого действия с сигнатурой, указанной в примере. Затем в действии выбрать префаб  персонажа (`` Action to Perform``)  и указать соответстующий делегат. **все взаимодействие внутри делегата должно происходить через поля ``owner`` и т.д.**

## Пример

```

public void GoToCover(AiContext context, UtilityPick pick) {
    if (pick.UtilityAction is PickerAction picker) {
        var choices = context.GetParameter(picker.evaluatedParamName) as List<Vector3>;
        var owner = context.owner as NpcMainScript;
        if (pick.SelectorIdx != -1) {
            owner._agent.SetDestination(choices[pick.SelectorIdx]);
        }
    }
}

public void GoToTarget(AiContext context, UtilityPick pick) {
    var hitGameObject = context.target;
    var owner = context.owner as NpcMainScript;
    if (owner != null) {
        owner._agent.SetDestination(hitGameObject.transform.position);
    }
}
```
