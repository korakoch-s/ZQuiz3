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

    constructor(private router: Router, private activeRoute: ActivatedRoute,
        private quizSvr: QuizService) {
        //this.questions = MockQuestions();
        this.quizSvr.quiz().then((qs) => {
            this.questions = qs;
            this.isQtLoad = true;
        });
        this.activeRoute.params.subscribe(params => {
            this.quizSvr.load(params['username']).then(tester => {
                this.tester = tester;
                this.isTesterLoad = true;
            });
        });
    }

    ngOnInit() {
        let timeOutCount: number = 0;
        const itid = setInterval(() => {
            if (timeOutCount > 20 || (this.isQtLoad && this.isTesterLoad)) {
                clearInterval(itid);
                this.mapTesterQuestions();
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
                target.Choice = new Choice();
                this.tester.TesterQuestions.push(target);
            } else {
                target.Question = qt;
                if (target.AnsChoiceId > 0) {
                    //already have answer
                    target.Choice = qt.Choices.find(ch => {
                        return ch.ChoiceId == target.AnsChoiceId;
                    });
                } else {
                    target.Choice = new Choice();
                }
            }

        });
    }

    submitClick() {
        this.quizSvr.submit(this.tester).then(tester => {
            console.log(JSON.stringify(this.tester));
            this.router.navigate(['/summary', this.tester.Name]);
        });
    }

    saveClick() {
        this.quizSvr.save(this.tester).then(obj => {
            this.router.navigate(['/register']);
        });
    }

}
