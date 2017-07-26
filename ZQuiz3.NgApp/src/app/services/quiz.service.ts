import { Injectable, Inject } from '@angular/core';
import { Tester, MockTester } from '../models/tester';
import { Question, MockQuestions } from '../models/question';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/do';

@Injectable()
export class QuizService {
    private currentTester: Tester;
    private apiUrl: string;

    constructor( @Inject('API_URL') apiUrl: string, private http: HttpClient) {
        this.apiUrl = apiUrl;
        console.log('API URL: ' + apiUrl);
    }

    public register(name: string): Promise<Tester> {
        //TODO: must implete real http service
        return new Promise<Tester>(resolve => {
            this.http.get(`${this.apiUrl}register/${name}`)
                .subscribe((obj: Tester) => {
                    this.currentTester = obj;
                    resolve(this.currentTester);
                });
        });
    }

    public load(name: string): Promise<Tester> {
        //TODO: must implete real http service

        return new Promise<Tester>(resolve => {
            if (!this.currentTester || this.currentTester.Name != name) {
                this.http.get(`${this.apiUrl}register/${name}`)
                    .subscribe((obj: Tester) => {
                        this.currentTester = obj;
                        resolve(this.currentTester);
                    });
            } else {
                resolve(this.currentTester);
            }
        });

    }

    public quiz(): Promise<Question[]> {
        //TODO: must implete real http service
        return new Promise<Question[]>(resolve => {
            this.http.get(`${this.apiUrl}quiz`)
                .subscribe((obj: Question[]) => {
                    //this.currentTester = new Tester();
                    //this.currentTester.fillFromJson(obj);
                    resolve(obj);
                });
        });

        //return MockQuestions();
    }

    public save(tester: Tester): Promise<any> {
        //TODO: must implete real http service

        tester.TesterQuestions.forEach(tq => {
            tq.AnsChoiceId = tq.Choice.ChoiceId;
        });

        return new Promise(resolve => {
            this.http.post(`${this.apiUrl}save`, tester)
                .subscribe((obj: any) => {
                    //this.currentTester = new Tester();
                    //this.currentTester.fillFromJson(obj);
                    resolve(obj);
                });
        });

        //this.currentTester = tester;
        //return tester;
    }

    public submit(tester: Tester): Promise<Tester> {
        //TODO: must implete real http service

        tester.TesterQuestions.forEach(tq => {
            tq.AnsChoiceId = tq.Choice.ChoiceId;
        });

        return new Promise<Tester>(resolve => {
            this.http.post(`${this.apiUrl}submit`, tester)
                .subscribe((obj: Tester) => {
                    this.currentTester = obj;
                    resolve(this.currentTester);
                });
        });

        //let score: number = 0;
        //let total: number = 0;
        //tester.TesterQuestions.forEach(itm => {
        //    if (itm.Choice) {
        //        score += itm.answer.score;
        //    }
        //    if (itm.question) {
        //        total += Math.max.apply(Math, itm.question.choices.map(ch => {
        //            return ch.score;
        //        }));
        //    }
        //});

        //tester.Score = 10;
        //tester.TotalScore = 50;
        //tester.Rank = 10;
        //tester.IsCompleted = true;

        //this.currentTester = tester;

        //return tester;
    }
}
