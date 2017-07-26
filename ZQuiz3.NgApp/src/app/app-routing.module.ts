import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RegisterComponent } from './components/register/register.component';
import { QuizComponent } from './components/quiz/quiz.component';
import { SummaryComponent } from './components/summary/summary.component';

const routes: Routes = [
    {
        path: '', redirectTo: 'register', pathMatch: 'full'
    },
    {
        path: 'register', component: RegisterComponent
    },
    {
        path: 'quiz/:username', component: QuizComponent
    },
    {
        path: 'summary/:username', component: SummaryComponent
    },
    {
        path: '**', redirectTo: 'register', pathMatch: 'full'
    }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }
