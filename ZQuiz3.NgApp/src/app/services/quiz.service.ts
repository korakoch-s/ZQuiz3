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
        return new Promise<Tester>((resolve, reject) => {
            this.http.get(`${this.apiUrl}register/${name}`)
                .subscribe((obj: Tester) => {
                    this.currentTester = obj;
                    resolve(this.currentTester);
                }, (err) => {
                    reject(err);
                });
        });
    }

    public load(name: string): Promise<Tester> {
        //TODO: must implete real http service

        return new Promise<Tester>((resolve, reject) => {
            if (!this.currentTester || this.currentTester.Name != name) {
                this.http.get(`${this.apiUrl}register/${name}`)
                    .subscribe((obj: Tester) => {
                        this.currentTester = obj;
                        resolve(this.currentTester);
                    }, (err) => {
                        reject(err);
                    });
            } else {
                resolve(this.currentTester);
            }
        });

    }

    public quiz(): Promise<Question[]> {
        return new Promise<Question[]>((resolve, reject) => {
            this.http.get(`${this.apiUrl}quiz`)
                .subscribe((obj: Question[]) => {
                    resolve(obj);
                }, (err) => {
                    reject(err);
                });
        });

    }

    public save(tester: Tester): Promise<any> {
        //TODO: must implete real http service

        tester.TesterQuestions.forEach(tq => {
            tq.AnsChoiceId = tq.Choice.ChoiceId;
        });

        return new Promise((resolve, reject) => {
            this.http.post(`${this.apiUrl}save`, tester)
                .subscribe((obj: any) => {
                    resolve(obj);
                }, (err) => {
                    reject(err);
                });
        });
    }

    public submit(tester: Tester): Promise<Tester> {

        tester.TesterQuestions.forEach(tq => {
            tq.AnsChoiceId = tq.Choice.ChoiceId;
        });

        return new Promise<Tester>((resolve, reject) => {
            this.http.post(`${this.apiUrl}submit`, tester)
                .subscribe((obj: Tester) => {
                    this.currentTester = obj;
                    resolve(this.currentTester);
                }, (err) => {
                    reject(err);
                });
        });
    }
}
