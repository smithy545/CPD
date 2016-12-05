---
layout: default
---
<div class="home">
  <h1 class="page-heading">evidence room</h1>
  <ul class="post-list">
    {% for post in site.posts.evidence %}
      <li>
        <a class="post-link" href="{{ post.url | prepend: site.baseurl }}"></a> 
        <span class="post-meta"></span> <br/>
          <p> {{ post.content }} </p>
        <p>
          <a class = "query-execute read-more"> FIND OUT </a>
        </p>
      </li>
    {% endfor %}
  </ul>
</div>
