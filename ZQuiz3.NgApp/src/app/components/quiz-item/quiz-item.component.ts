import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Question, Choice } from '../../models/question';
import { TesterQuestion } from '../../models/tester';

@Component({
    selector: 'app-quiz-item',
    templateUrl: './quiz-item.component.html',
    styleUrls: ['./quiz-item.component.css']
})
export class QuizItemComponent implements OnInit {
    @Input() question: Question;
    @Input() itemNo: number;
    @Input() answer: Choice;
    @Output() answerChange: EventEmitter<Choice> = new EventEmitter<Choice>();

    constructor() { }

    ngOnInit() {
    }

    choiceClick(ans: Choice) {
        this.answer = ans;
        this.answerChange.emit(ans);
    }

}
