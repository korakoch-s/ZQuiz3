export class BaseModel {
    fillFromJson(json: string) {
        let jsonObj = JSON.parse(json);
        for (let propName in jsonObj) {
            this[propName] = jsonObj[propName]
        }
    }
}