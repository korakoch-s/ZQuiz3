import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Question, MockQuestions, Choice } from '../../models/question';
import { Tester, TesterQuestion, MockTester } from '../../models/tester';
import { QuizService } from '../../services/quiz.service';

@Component({
    selector: 'app-quiz',
    templateUrl: './quiz.component.html',
    styleUrls: ['./quiz.component.css']
})
export class QuizComponent implements OnInit {
    public questions: Question[];
    public tester: Tester;

    private isQtLoad: boolean = false;
    private isTesterLoad: boolean = false;

    public isWorking: boolean = false;

    constructor(private router: Router, private activeRoute: ActivatedRoute,
        private quizSvr: QuizService) {
        //this.questions = MockQuestions();
        this.isWorking = true;
        this.quizSvr.quiz().then((qs) => {
            this.questions = qs;
            this.isQtLoad = true;
        }).catch(e => {
            this.isQtLoad = true;
            console.log('Some error: ' + JSON.stringify(e));
        });
        this.activeRoute.params.subscribe(params => {
            this.quizSvr.load(params['username']).then(tester => {
                this.tester = tester;
                this.isTesterLoad = true;
            }).catch(e => {
                this.isTesterLoad = true;
                console.log('Some error: ' + JSON.stringify(e));
            });
        });
    }

    ngOnInit() {
        let timeOutCount: number = 0;
        const itid = setInterval(() => {
            if (timeOutCount > 20 || (this.isQtLoad && this.isTesterLoad)) {
                clearInterval(itid);
                this.mapTesterQuestions();
                this.isWorking = false;
            }
            timeOutCount++;
        }, 500);
    }

    private mapTesterQuestions() {
        if (!(this.isQtLoad && this.isTesterLoad)) {
            console.log('Timeout loading questions and tester data.');
            return;
        }

        this.questions.forEach(qt => {
            let target = this.tester.TesterQuestions.find(tq => {
                return tq.QuestionId === qt.QuestionId;
            });

            if (!target) {
                target = new TesterQuestion();
                target.QuestionId = qt.QuestionId;
                target.Question = qt;
                target.Choice = new Choice(qt.QuestionId);
                this.tester.TesterQuestions.push(target);
            } else {
                target.Question = qt;
                if (target.AnsChoiceId > 0) {
                    //already have answer
                    target.Choice = qt.Choices.find(ch => {
                        return ch.ChoiceId == target.AnsChoiceId;
                    });
                } else {
                    target.Choice = new Choice(qt.QuestionId);
                }
            }

        });
    }

    submitClick() {
        this.isWorking = true;
        this.quizSvr.submit(this.tester).then(tester => {
            this.router.navigate(['/summary', this.tester.Name]);
            this.isWorking = false;
        }).catch(e => {
            this.isQtLoad = true;
            console.log('Some error: ' + JSON.stringify(e));
            this.isWorking = false;
        });
    }

    saveClick() {
        this.isWorking = true;
        this.quizSvr.save(this.tester).then(obj => {
            this.router.navigate(['/register']);
            this.isWorking = false;
        }).catch(e => {
            this.isQtLoad = true;
            console.log('Some error: ' + JSON.stringify(e));
            this.isWorking = false;
        });
    }

}
