import { Question, Choice, MockQuestions } from './question';
import { BaseModel } from './base-model'; 

export class Tester extends BaseModel {
    public TesterId: number;
    public Name: string;
    public IsCompleted: boolean;
    public Score: number;
    public TotalScore: number;
    public Rank: number;
    public TesterQuestions: TesterQuestion[];

    constructor(id?: number, name?: string) {
        super();
        this.TesterId = id || 0;
        this.Name = name || undefined;
    }
}

export class TesterQuestion extends BaseModel {
    public Question: Question;
    public Choice: Choice;
    public TesterId: number;
    public QuestionId: number;
    public AnsChoiceId: number;
}

export const MockTester = (name: string): Tester => {
    let tester: Tester = new Tester(1, name);
    let questions: Question[] = MockQuestions();

    tester.Score = 20;
    tester.TotalScore = 50;
    tester.Rank = 10;
    tester.TesterQuestions = [];

    questions.forEach(q => {
        let newTq = new TesterQuestion();
        newTq.Question = q;
        newTq.Choice = new Choice();
        tester.TesterQuestions.push(newTq);
    })
    tester.TesterQuestions[0].Choice = questions[0].Choices[0];
    tester.TesterQuestions[1].Choice = questions[1].Choices[2];
    
    return tester;
}